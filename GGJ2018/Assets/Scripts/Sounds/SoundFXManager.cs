using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundFXManager : MonoBehaviour {

    public static SoundFXManager GamesSoundFXManager; 
   // public List<SFXDef> AudioFiles = new List<SFXDef>();

    public List<AudioClip> AudioClips = new List<AudioClip>();

    List<SFXDef> Definitions = new List<SFXDef>(); 

    AudioSource AudioSource; 

	// Use this for initialization
	void Start () {
        AudioSource = GetComponent<AudioSource>();
        LoadDefinitions(); 
        GamesSoundFXManager = this; 

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void LoadDefinitions()
    {
        foreach(var clip in AudioClips)
        {
            var def = new SFXDef();
            def.AudioClip = clip;
            def.Name = clip.name;

            if (def.Name.Contains("Zombie Noise"))
                def.Type = SFXType.Zombie;
            else if (def.Name.Contains("Transform"))
                def.Type = SFXType.Transform;
            else if (def.Name.Contains("Hit"))
                def.Type = SFXType.Hit;
            else
                def.Type = SFXType.NONE; 


            Definitions.Add(def); 
        }
    }

    public void PlaySound(SFXType type)
    {
        var sound = Definitions.Where(x => x.Type == type).RandomElement();
        if (sound == null)
            return;

        PlaySound(sound.AudioClip); 
    }

    public void PlaySound(string soundName)
    {
        var sound = AudioClips.FirstOrDefault(x => x.name == soundName);
        if (sound == null)
            return;

        PlaySound(sound); 
    }


    protected void PlaySound(AudioClip clip)
    {
        if (!AudioSource.isPlaying)
        {
            AudioSource.PlayOneShot(clip);
        }
       // AudioSource.PlayClipAtPoint(clip, location); 
    }


}
