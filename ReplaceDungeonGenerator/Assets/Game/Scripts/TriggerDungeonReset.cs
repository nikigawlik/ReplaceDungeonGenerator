using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDungeonReset : MonoBehaviour
{
    public ReplaceDungeonGenerator.RecipeReplacer recipeReplacer;
    public ReplaceDungeonGenerator.RandomReplacer randReplacer;

    private void Start() {
        GetComponent<Interactible>().onInteract.AddListener(Interact);
        recipeReplacer = GameObject.Find("Generator").GetComponent<ReplaceDungeonGenerator.RecipeReplacer>();
        randReplacer = GameObject.Find("Generator").GetComponent<ReplaceDungeonGenerator.RandomReplacer>();
    }

    private void Interact() {
        if(recipeReplacer != null) {
            recipeReplacer.seed = Random.Range(0, int.MaxValue);
            recipeReplacer.Generate();
        }
        if(randReplacer != null) {
            randReplacer.seed = Random.Range(0, int.MaxValue);
            randReplacer.Generate();
        }
    }
}
