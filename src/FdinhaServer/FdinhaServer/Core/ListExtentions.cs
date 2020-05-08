using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FdinhaServer.Core
{
    public static class ListExtentions
    {
        public static Stack<T> ToStack<T>(this IList<T> list)
        {
            var stack = new Stack<T>();
            foreach (var l in list){
                stack.Push(l);
            }

            return stack;
        }
    }
}
