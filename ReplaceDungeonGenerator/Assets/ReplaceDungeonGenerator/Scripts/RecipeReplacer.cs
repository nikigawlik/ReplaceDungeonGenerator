using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

namespace ReplaceDungeonGenerator
{
	/// This class wraps ReplacementEngine, but reads the order of rules from a recipe file
    [RequireComponent(typeof(PatternView))]
    [RequireComponent(typeof(ReplacementEngine))]
	[ExecuteInEditMode]
	public class RecipeReplacer : MonoBehaviour {
		private enum InstructionType {
			Skip, // instruction that skips to after the next return, this skips function definitions white executing
			Goto, // instruction jumps somewhere in the instruction array, and pushes current pos to stack
			Return, // returns to position popped from stack
			Repeat, // repeats the next instruction n times
			ApplyRule, // applies a rule
			ApplyRuleRange, // applies a rule allowing for partial matches
			Error, // just an error, to keep track, will be ignored
		}

		private class Instruction {
			public InstructionType type;
			public int gotoPosition;
			public string filter;
			public int minRepeats;
			public int maxRepeats;

			public Instruction(InstructionType type) {
				this.type = type;
			}
		}

		private enum GenerationStepResult
		{
			VisibleChange,
			NoVisibleChange,
			EndOfFile
		}

		private struct ProgramState {
			public int instruction;
			public int applicationCounter;
			public ProgramState(int instruction, int applicationCounter) {
				this.instruction = instruction;
				this.applicationCounter = applicationCounter;
			}
		}
		
		public TextAsset recipeFile;
		public int maxGenerationSteps = 10;
		public PatternView patternView;
        public bool increaseSeed = true;
        public int seed = 0;

		private int currentInstruction = 0;
		private Stack<ProgramState> gotoStack;
		private Instruction[] instructions;
		private int applicationCounter;

        /// Do repeated replacement steps until no replacements are possible 
        /// or the maximum step number is reached
		[Button]
        public void Generate()
        {
            ReplacementEngine re = GetComponent<ReplacementEngine>();
            InitializeGeneration();

            if (re != null)
            {
                for (int i = 0; i < maxGenerationSteps; i++)
                {
                    if (GenerationStepInternal() == GenerationStepResult.EndOfFile)
                    {
                        break;
                    }
                }
            }
        }

		[Button]
        public void InitializeGeneration()
        {
            if(increaseSeed) seed++;
            Random.InitState(seed);

			patternView.pattern = new Pattern(patternView.pattern.Size, Tile.Empty);
            ReplacementEngine re = GetComponent<ReplacementEngine>();
            re.SetStartSymbol();
            PatternView.UpdateView();

			ParseInstructions();
			currentInstruction = 0;
			applicationCounter = 0;
			gotoStack = new Stack<ProgramState>();
        }

		[Button]
        public void GenerateStep()
        {
			// we execute until we have visible change
			for(int i = 0; i < 100; i++) {
				GenerationStepResult result = GenerationStepInternal();
				if(result == GenerationStepResult.EndOfFile) {
					return;
				}
            	if(result == GenerationStepResult.VisibleChange) {
					break;
				}
			}
            PatternView.UpdateView();
        }

		private void OnValidate() {
            // assign pattern view automatically
			if(patternView == null) {
				patternView = GetComponent<PatternView>();
			}
		}

		private GenerationStepResult GenerationStepInternal() {
            ReplacementEngine re = GetComponent<ReplacementEngine>();
			Debug.Log("Generation step, instruction " + currentInstruction + ", applications left: " + applicationCounter);

			if(currentInstruction >= instructions.Length) {
				return GenerationStepResult.EndOfFile;
			}

			bool visibleChange = false;
			Instruction instr = instructions[currentInstruction];
			switch (instr.type) {
				case InstructionType.Skip:
				// skip to after next return
					while(currentInstruction < instructions.Length 
					&& instructions[currentInstruction].type != InstructionType.Return) {
						currentInstruction++;
					}
					currentInstruction++;
				break;
				case InstructionType.Return:
					if(gotoStack.Count > 0) {
						ProgramState ps = gotoStack.Pop();
						currentInstruction = ps.instruction;
						applicationCounter = ps.applicationCounter;
					}
				break;
				case InstructionType.Repeat:
					// we just set the counter, the execution is handled by the repeatable instructions
					applicationCounter = Mathf.Max(1, Random.Range(instr.minRepeats, instr.maxRepeats));
					currentInstruction++;
				break;
				// repeatable instructions
				case InstructionType.Goto:
					if(UseApplication()) {
						gotoStack.Push(new ProgramState(currentInstruction, applicationCounter));
						currentInstruction = instr.gotoPosition;
					} else {
						currentInstruction++;
					}
				break;
				case InstructionType.ApplyRule:
				case InstructionType.ApplyRuleRange:
					if(UseApplication()) {
						// apply rule once and count down
						visibleChange = re.ReplaceRandomMatch(instr.filter, instr.type == InstructionType.ApplyRuleRange);

					} else {
						currentInstruction++;
					}
				break;
				case InstructionType.Error:
					currentInstruction++;
				break;
			}

			return visibleChange? GenerationStepResult.VisibleChange : GenerationStepResult.NoVisibleChange;
		}

		private bool UseApplication() {
			if(applicationCounter <= 0) return false;
			applicationCounter--;
			return true;
		}
		
		/// This function parses the text into an array of instruction objects that can be easily executed in series
		private void ParseInstructions() {
			string str = recipeFile.text;
			string[] lines = str.Split('\n');
			List<Instruction> instructionList = new List<Instruction>();
			Dictionary<string, int> markers = new Dictionary<string, int>();

			foreach (string l in lines)
			{
				string line = l.Trim();
				if(line.Length == 0) continue;
				// comment check
				if(line[0] == '#') continue;
				string[] tokens = line.Split(null);
				// empty line check
				if(tokens.Length == 0) continue;

				string firstToken = tokens[0];
				switch(firstToken) {
					case "def":
						if(tokens.Length < 2) continue; // no name for def
						// add a Skip to skip the function body
						instructionList.Add(new Instruction(InstructionType.Skip));
						// and register a marker for later use
						markers[tokens[1]] = instructionList.Count;
						
					break;
					case "end":
						// For end add a return intruction
						instructionList.Add(new Instruction(InstructionType.Return));
					break;
					default:
						// default means any symbol type command, like a function or rule
						Instruction instr;
						// check for a function call
						if(markers.ContainsKey(firstToken)) {
							// function call, use a goto instruction
							instr = new Instruction(InstructionType.Goto);
							instr.gotoPosition = markers[firstToken];
						} else {
							// check if it's a rule range
							if(firstToken.EndsWith("*")) {
								// rule range instruction
								instr = new Instruction(InstructionType.ApplyRuleRange);
								// remove the *
								tokens[0] = firstToken = firstToken.Substring(0, firstToken.Length - 1);
							} else {
								// normal application instruction
								instr = new Instruction(InstructionType.ApplyRule);
							}
							// set the filter
							instr.filter = firstToken;
						}
						// try to parse ints
						Instruction repeatInstruction = new Instruction(InstructionType.Repeat);
						try{
							if(tokens.Length == 1) {
								// single application
								repeatInstruction.minRepeats = repeatInstruction.maxRepeats = 1;
							} else if(tokens.Length == 2) {
								// fixed number of applications
								repeatInstruction.minRepeats = repeatInstruction.maxRepeats = int.Parse(tokens[1]);
							} else {
								// random number of applications
								repeatInstruction.minRepeats = int.Parse(tokens[1]);
								repeatInstruction.maxRepeats = int.Parse(tokens[2]);
							}
						} catch {
							// parse error, just ignore line
							instructionList.Add(new Instruction(InstructionType.Error));
							continue;
						}
						// combination of the repeat instruction and the execting instruction
						instructionList.Add(repeatInstruction);
						instructionList.Add(instr);
					break;
				}
			}

			instructions = instructionList.ToArray();

			string debugString = "Parsed intructions (line type gotoPosition filter minApplications maxApplications):";
			for(int i = 0; i < instructions.Length; i++)
			{
				Instruction instr = instructions[i];
				debugString += "\n" + i + " - " + 
					" " + instr.type +
					" " + instr.gotoPosition +
					" " + instr.filter +
					" " + instr.minRepeats +
					" " + instr.maxRepeats;
			}
			Debug.Log(debugString);
		}
	}
}