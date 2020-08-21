using ICSharpCode.NRefactory.Ast;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameplayIngredients.Rigs
{
    public abstract class Rig : MonoBehaviour
    {
        public UpdateMode updateMode
        {
            get { return m_UpdateMode; }
        }

        public int rigPriority
        {
            get { return m_RigPriority; }
        }

        public enum UpdateMode
        {
            Update,
            LateUpdate,
            FixedUpdate,
        }

        public abstract UpdateMode defaultUpdateMode { get; }
        public abstract int defaultPriority { get; }
        public virtual bool canChangeUpdateMode { get { return false; } }

        protected bool CanChangeUpdateMode() { return canChangeUpdateMode; }

        [SerializeField, EnableIf("CanChangeUpdateMode")]
        private UpdateMode m_UpdateMode;
        [SerializeField]
        private int m_RigPriority = 0;


        private void Reset()
        {
            if(!canChangeUpdateMode)
            m_UpdateMode = defaultUpdateMode;
            m_RigPriority = defaultPriority;
        }

        protected virtual void OnEnable()
        {
            Manager.Get<RigManager>().RegistedRig(this);
        }

        protected virtual void OnDisable()
        {
            Manager.Get<RigManager>().RemoveRig(this);
        }

        public abstract void UpdateRig(float deltaTime);

    }
}


