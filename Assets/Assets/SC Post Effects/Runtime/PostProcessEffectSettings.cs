﻿using UnityEngine;

//Dummy class, but ensures all classes derrived from the PPS stack are serialized as ScriptableObjects when it is not present
#if !PPS
public class PostProcessEffectSettings : ScriptableObject {}
#endif
