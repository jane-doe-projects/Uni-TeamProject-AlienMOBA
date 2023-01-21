using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSoundControl : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    [SerializeField] AudioSource matchStartedSound;

    // match outcome sounds
    [SerializeField] AudioSource victorySound;
    [SerializeField] AudioSource defeatSound;

    // hero sounds
    [SerializeField] AudioSource levelUpSound;
    [SerializeField] AudioSource respawnSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource projectileImpactSound;

    // hero ability sounds
    [SerializeField] AudioSource abilityNotUsableSound;
    [SerializeField] AudioSource ability1Sound;
    [SerializeField] AudioSource ability2Sound;
    [SerializeField] AudioSource ability3Sound;
    [SerializeField] AudioSource ability4Sound;

    // building sounds
    [SerializeField] AudioSource turretDestructionSound;
    [SerializeField] AudioSource turretShotSound;

    public void MatchStarted() { matchStartedSound.PlayOneShot(matchStartedSound.clip);  }

    public void Victory() { victorySound.PlayOneShot(victorySound.clip); }
    public void Defeat() { defeatSound.PlayOneShot(defeatSound.clip); }

    public void LevelUp() { levelUpSound.PlayOneShot(levelUpSound.clip); }
    public void Respawn() { respawnSound.PlayOneShot(respawnSound.clip); }
    public void Death() { deathSound.PlayOneShot(deathSound.clip); }
    public void ProjectileImpact() { projectileImpactSound.PlayOneShot(projectileImpactSound.clip); }

    public void AbilityNotUseable() { abilityNotUsableSound.PlayOneShot(abilityNotUsableSound.clip); }
    public void Ability1() { ability1Sound.PlayOneShot(ability1Sound.clip); }
    public void Ability2() { ability2Sound.PlayOneShot(ability2Sound.clip); }
    public void Ability3() { ability3Sound.PlayOneShot(ability3Sound.clip); }
    public void Ability4() { ability4Sound.PlayOneShot(ability4Sound.clip); }

    public void TurretDestruction() { turretDestructionSound.PlayOneShot(turretDestructionSound.clip); }
    public void TurretShot() { turretShotSound.PlayOneShot(turretShotSound.clip); }

}
