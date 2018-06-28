using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Regulus.Unity.Editor;


namespace Regulus.Unity.Tests
{

    public class Test
    {
        public event System.Action<int> Event1;
        public int Method1()
        {
            return 0;
        }
    }
}


public class MonoBehaviourCodeGeneratorTest
{

    


    [Test]
    public void NewTestScriptSimplePasses()
    {

        
        var testCode =
@"namespace Regulus.Unity.MonoBehaviourProxy.Regulus.Unity.Tests
{
    public class Test : UnityEngine.MonoBehaviour
    {

        private global::Regulus.Unity.Tests.Test _Core;
        public void Initial(global::Regulus.Unity.Tests.Test core)
        {
            _Core = core;
            _Core.Event1 += _OnEvent1;
        }
        public void Release()
        {
            _Core.Event1 -= _OnEvent1;
        }
        public global::System.Int32 Method1()
        {
            return _Core.Method1();
        }
        
        [System.Serializable]
        public class UnityEvent1 : UnityEngine.Events.UnityEvent<global::System.Int32> {}
        public UnityEvent1 Event1;
        
        
        private void _OnEvent1(global::System.Int32 arg0)
        {
            Event1.Invoke(arg0);
        }
        
    }
}";

        var codeGenerator = new Regulus.Unity.Editor.MonoBehaviourCodeGenerator(typeof(Regulus.Unity.Tests.Test));
        Assert.AreEqual( testCode.Replace("\r","").Replace("\n","") , codeGenerator.Code.Replace("\r", "").Replace("\n", ""));
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
