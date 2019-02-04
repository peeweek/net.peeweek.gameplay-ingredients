using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayIngredients
{
    public static class Messager
    {
        public delegate void Message();

        private static Dictionary<string, Message> m_RegisteredMessages;

        static Messager()
        {
            m_RegisteredMessages = new Dictionary<string, Message>();
        }

        public static void RegisterMessage(string eventName, Message message)
        {
            if (!m_RegisteredMessages.ContainsKey(eventName))
                m_RegisteredMessages.Add(eventName, message);
            else
                m_RegisteredMessages[eventName] += message;
        }

        public static void RemoveMessage(string eventName, Message message)
        {
            var currentEvent = m_RegisteredMessages[eventName];
            currentEvent -= message;
            if (currentEvent == null || currentEvent.GetInvocationList().Length == 0)
                m_RegisteredMessages.Remove(eventName);
        }

        public static void Send(string eventName)
        {
            Debug.Log(string.Format("[MessageManager] Broadcast: {0}", eventName));

            if (m_RegisteredMessages.ContainsKey(eventName))
            {
                try
                {
                    var call = m_RegisteredMessages[eventName];
                    var list = call.GetInvocationList();

                    if (call != null)
                        call();
                    else
                        Debug.LogWarning(string.Format("Found null action while sending Message {0}", eventName));

                }
                catch (Exception e)
                {
                    Debug.LogError(string.Format("Messager : Caught {0} while sending Message {1}", e.GetType().Name, eventName));
                    Debug.LogException(e);
                }
            }
            else
            {
                Debug.Log("[MessageManager] could not find any listeners for event : " + eventName);
            }
        }
    }
}

