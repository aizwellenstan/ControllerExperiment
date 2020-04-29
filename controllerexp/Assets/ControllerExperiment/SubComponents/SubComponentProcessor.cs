﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ControllerExperiment.SubComponents
{
    public class SubComponentProcessor : MonoBehaviour
    {
        private ControllerEntity m_owner;

        public ControllerEntity owner
        {
            get
            {
                if (m_owner == null)
                {
                    m_owner = this.gameObject.GetComponentInParent<ControllerEntity>();
                }
                return m_owner;
            }
        }

        public Dictionary<int, Process> SetDic = new Dictionary<int, Process>();
        public delegate void Process();

        Dictionary<int, SetEntityFloat> SetFloatDic = new Dictionary<int, SetEntityFloat>();
        public delegate void SetEntityFloat(float f);

        Dictionary<int, SetEntityBool> SetBoolDic = new Dictionary<int, SetEntityBool>();
        public delegate void SetEntityBool(bool b);

        Dictionary<int, GetBoolDelegate> GetBoolDic = new Dictionary<int, GetBoolDelegate>();
        public delegate bool GetBoolDelegate();

        Dictionary<int, GetIntDelegate> GetIntDic = new Dictionary<int, GetIntDelegate>();
        public delegate int GetIntDelegate();

        [Header("Debug")]
        public List<SubComponent> SubComponents = new List<SubComponent>();

        private void Awake()
        {
            SubComponents.Clear();

            SubComponent[] arr = this.gameObject.GetComponentsInChildren<SubComponent>();

            foreach(SubComponent s in arr)
            {
                SubComponents.Add(s);

                // check whether the subcomponent needs to be updated/fixedupdated
                System.Type child = s.GetType();
                s.DoFixedUpdate = OverrideCheck.IsOverridden(child, typeof(SubComponent), "OnFixedUpdate");
                s.DoUpdate = OverrideCheck.IsOverridden(child, typeof(SubComponent), "OnUpdate");
            }
        }

        public void FixedUpdateSubComponents()
        {
            foreach(SubComponent s in SubComponents)
            {
                if (s.DoFixedUpdate)
                {
                    s.OnFixedUpdate();
                }
            }
        }

        public void UpdateSubComponents()
        {
            foreach (SubComponent s in SubComponents)
            {
                if (s.DoUpdate)
                {
                    s.OnUpdate();
                }
            }
        }

        public void SetFloat(int key, float f)
        {
            if (SetFloatDic.ContainsKey(key))
            {
                SetFloatDic[key](f);
            }
            else
            {
                Debug.LogError("SetFloat function not found");
            }
        }

        public void DelegateSetFloat(int key, SetEntityFloat del)
        {
            SetFloatDic.Add(key, del);
        }

        public void SetBool(int key, bool b)
        {
            if (SetBoolDic.ContainsKey(key))
            {
                SetBoolDic[key](b);
            }
            else
            {
                Debug.LogError("SetBool function not found");
            }
        }

        public void DelegateSetBool(int key, SetEntityBool del)
        {
            SetBoolDic.Add(key, del);
        }

        public bool GetBool(int key)
        {
            if (GetBoolDic.ContainsKey(key))
            {
                return GetBoolDic[key]();
            }
            else
            {
                Debug.LogError("GetBool function not found");
                return false;
            }
        }

        public void DelegateGetBool(int key, GetBoolDelegate del)
        {
            GetBoolDic.Add(key, del);
        }

        public int GetInt(int key)
        {
            if (GetIntDic.ContainsKey(key))
            {
                return GetIntDic[key]();
            }
            else
            {
                Debug.LogError("GetInt function not found");
                return 0;
            }
        }

        public void DelegateGetInt(int key, GetIntDelegate del)
        {
            GetIntDic.Add(key, del);
        }
    }
}