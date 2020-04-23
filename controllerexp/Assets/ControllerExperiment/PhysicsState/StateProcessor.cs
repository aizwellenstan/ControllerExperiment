﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ControllerExperiment.PhysicsState
{
    public class StateProcessor : MonoBehaviour
    {
        public List<PhysicsState> AllStates = new List<PhysicsState>();
        public PhysicsState Current = null;

        void InitProcessor(System.Type type)
        {
            Debug.Log("State initialized: " + type.Name);

            GameObject obj = new GameObject();
            obj.transform.parent = this.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            obj.name = type.Name;
            obj.AddComponent(type);

            PhysicsState newState = obj.GetComponent<PhysicsState>();
            
            if (!AllStates.Contains(newState))
            {
                AllStates.Add(newState);
            }

            TransitionTo(newState.GetType());
        }

        public void TransitionTo(System.Type type)
        {
            Debug.Log("Attempting transition to " + type.Name + "..");

            PhysicsState s = GetState(type);

            if (s == null)
            {
                Debug.Log(type.Name + " is null. Initiating..");
                InitProcessor(type);
            }
            else
            {
                Debug.Log("Transitioned to: " + type.Name);
                Current = s;
            }
        }

        PhysicsState GetState(System.Type type)
        {
            foreach(PhysicsState s in AllStates)
            {
                if (s.GetType() == type)
                {
                    return s;
                }
            }

            return null;
        }

        public void FixedUpdateState()
        {
            Current.ProcFixedUpdate();
        }
    }
}