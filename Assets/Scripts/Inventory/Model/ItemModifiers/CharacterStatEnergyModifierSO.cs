using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Items/Modifiers/Energy Modifier", fileName = "Energy Modifier")]
public class CharacterStatEnergyModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        if (character.TryGetComponent<Player>(out var player))
            player.Energy += (int)val;
    }
}
