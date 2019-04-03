using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo6_RandomSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // break on set in 3d party code: System.Windows.Forms.ButtonBase.set_Text
            // use the same technique to break on all overloads of methods
            var button = new Button() { Text = "And now you know how to do it!" }; // C:\Users\Alexey_Polyakov\Documents\Visual Studio 2013\Visualizers\autoexp.cs
            var b = new A();
            var c = new B();
            var json = "{\"name\":\"alexey\", \"date\": \"today\" }";
            var xml = "<root><element><value>16</value></element></root>";
            
            Debugger.Break();

            Recursion(10);

            try
            {
                A1();
            }
            catch (Exception e)
            { // exception is available in OzCode even after debugging has stopped
                Debugger.Break();
            }

            var button2 = new Button(); // ozCode - show all instances

            Debugger.Break();
        }

        #region Debug representation

        [DebuggerDisplayAttribute("Magic is {Val}")]
        public class A
        {
            public int Val
            {
                get { return 42; }
            }
        }

        [DebuggerTypeProxy(typeof(BProxy))]
        public class B
        {
            public int Val
            {
                get { return 42; }
            }
        }

        public class BProxy
        {
            public B Val { get; set; }

            public BProxy(B val)
            {
                Val = val;
            }

            public string RealValue
            {
                get { return "42 is not the answer!"; }
            }
        }

        #endregion //Debug representation

        #region Recursion

        public static void Recursion(int i)
        { // easy to see in Parallel Watch and CallStack windows
            if (i < 0)
            {
                Debugger.Break();
                return;
            }
            Recursion(i -1);
        }

        #endregion


        public static void A1()
        {
            try
            {
                B1();

            }
            catch (Exception e)
            {
                throw new Exception("Important!!!", e);
            }
        }

        public static void B1()
        {
            C1();
        }

        public static void C1()
        {
            var result = 1/int.Parse("0");
        }
    }
}
