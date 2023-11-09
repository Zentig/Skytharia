using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Items/Modifiers/Health Modifier", fileName = "Health Modifier")]

public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        if (character.TryGetComponent<Player>(out var player))
            player.Health += (int)val;
    }
}
