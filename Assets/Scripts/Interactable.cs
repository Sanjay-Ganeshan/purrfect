﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    bool Interact(Player p);
    
    bool Interact(Cat c);
}
