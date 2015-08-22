///<summary>
/// The RedBlackEnumerator class returns the keys or data objects of the treap in
/// sorted order. 
///</summary>
using System;
using System.Collections;

namespace CTPPV5.Infrastructure.Collections
{
	public class RedBlackEnumerator
	{
		// the treap uses the stack to order the nodes
		private Stack stack;
		// return the keys
		private bool keys;
		// return in ascending order (true) or descending (false)
		private bool ascending;
		
		// key
		private IComparable ordKey;
		// the data or value associated with the key
		private object objValue;
        private RedBlack redBlack;

        public  string  Color;             // testing only, don't use in live system
        public  IComparable parentKey;     // testing only, don't use in live system
		
		///<summary>
		///Key
		///</summary>
		public IComparable Key
		{
			get
            {
				return ordKey;
			}
			
			set
			{
				ordKey = value;
			}
		}
		///<summary>
		///Data
		///</summary>
		public object Value
		{
			get
            {
				return objValue;
			}
			
			set
			{
				objValue = value;
			}
		}
		
		///<summary>
		/// Determine order, walk the tree and push the nodes onto the stack
		///</summary>
		public RedBlackEnumerator(RedBlack redBlack, RedBlackNode tnode, bool keys, bool ascending) 
        {

            this.stack = new Stack();
            this.keys = keys;
            this.ascending = ascending;
            this.redBlack = redBlack;
			
            // use depth-first traversal to push nodes into stack
            // the lowest node will be at the top of the stack
            if(ascending)
			{   // find the lowest node
                while (tnode != redBlack.sentinelNode)
				{
					stack.Push(tnode);
					tnode = tnode.Left;
				}
			}
			else
			{
                // the highest node will be at top of stack
                while (tnode != redBlack.sentinelNode)
				{
					stack.Push(tnode);
					tnode = tnode.Right;
				}
			}
			
		}
		///<summary>
		/// HasMoreElements
		///</summary>
		public bool HasMoreElements()
		{
			return (stack.Count > 0);
		}
		///<summary>
		/// NextElement
		///</summary>
		public object NextElement()
		{
			if(stack.Count == 0)
				throw(new RedBlackException("Element not found"));
			
			// the top of stack will always have the next item
			// get top of stack but don't remove it as the next nodes in sequence
			// may be pushed onto the top
			// the stack will be popped after all the nodes have been returned
			RedBlackNode node = (RedBlackNode) stack.Peek();	//next node in sequence
			
            if(ascending)
            {
                if (node.Right == redBlack.sentinelNode)
                {	
                    // yes, top node is lowest node in subtree - pop node off stack 
                    RedBlackNode tn = (RedBlackNode) stack.Pop();
                    // peek at right node's parent 
                    // get rid of it if it has already been used
                    while(HasMoreElements()&& ((RedBlackNode) stack.Peek()).Right == tn)
                        tn = (RedBlackNode) stack.Pop();
                }
                else
                {
                    // find the next items in the sequence
                    // traverse to left; find lowest and push onto stack
                    RedBlackNode tn = node.Right;
                    while (tn != redBlack.sentinelNode)
                    {
                        stack.Push(tn);
                        tn = tn.Left;
                    }
                }
            }
            else            // descending, same comments as above apply
            {
                if (node.Left == redBlack.sentinelNode)
                {
                    // walk the tree
                    RedBlackNode tn = (RedBlackNode) stack.Pop();
                    while(HasMoreElements() && ((RedBlackNode)stack.Peek()).Left == tn)
                        tn = (RedBlackNode) stack.Pop();
                }
                else
                {
                    // determine next node in sequence
                    // traverse to left subtree and find greatest node - push onto stack
                    RedBlackNode tn = node.Left;
                    while (tn != redBlack.sentinelNode)
                    {
                        stack.Push(tn);
                        tn = tn.Right;
                    }
                }
            }
			
			// the following is for .NET compatibility (see MoveNext())
            Key = node.Key;
            Value = node.Data;
            // ******** testing only ********
            try
            {
                parentKey = node.Parent.Key;            // testing only
            }
            catch(Exception e)
            {
                object o = e;                       // stop compiler from complaining
                parentKey = 0;
            }
			if(node.Color == 0)                     // testing only
                Color = "Red";
            else
                Color = "Black";
            // ******** testing only ********

            return keys == true ? node.Key : node.Data;			
		}
		///<summary>
		/// MoveNext
		/// For .NET compatibility
		///</summary>
		public bool MoveNext()
		{
			if(HasMoreElements())
			{
				NextElement();
				return true;
			}
			return false;
		}
	}
}
