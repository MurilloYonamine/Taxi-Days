using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationQueue
    {
        /*
        A queue of conversations to be
        run by the conversation manager.
        */

        private Queue<Conversation> conversationQueue = new Queue<Conversation>();
        public Conversation topConversation => conversationQueue.Peek();

        public void Enqueue(Conversation conversation) => conversationQueue.Enqueue(conversation);
        public void EnqueuePriority(Conversation conversation)
        {
            Queue<Conversation> queue = new Queue<Conversation>();
            queue.Enqueue(conversation);

            while (conversationQueue.Count > 0) queue.Enqueue(conversationQueue.Dequeue());

            conversationQueue = queue;
        }
        public void Dequeue()
        {
            if (conversationQueue.Count > 0) conversationQueue.Dequeue();
        }
        public bool IsEmpty() => conversationQueue.Count == 0;
    }
}