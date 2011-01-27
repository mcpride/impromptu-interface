// 
//  Copyright 2010  Ekon Benefits
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Runtime.Serialization;
using NUnit.Framework;
using ImpromptuInterface;
using System.Dynamic;
namespace UnitTestImpromptuInterface
{
	[TestFixture()]
	public class Basic:AssertionHelper
	{
	 

     

		[Test]
        public void AnonPropertyTest()
        {
            var tAnon = new {Prop1 = "Test", Prop2 = 42L, Prop3 = Guid.NewGuid()};

            var tActsLike = tAnon.ActLike<ISimpeleClassProps>();

		   
            Assert.AreEqual(tAnon.Prop1,tActsLike.Prop1);
            Assert.AreEqual(tAnon.Prop2, tActsLike.Prop2);
            Assert.AreEqual(tAnon.Prop3, tActsLike.Prop3);
        }
		
		

        [Test]
        public void CacheTest()
        {
            var tAnon = new { Prop1 = "Test 1", Prop2 = 42L, Prop3 = Guid.NewGuid() };
            var tAnon2 = new { Prop1 = "Test 2", Prop2 = 43L, Prop3 = Guid.NewGuid() };

            var tActsLike = tAnon.ActLike<ISimpeleClassProps>();
            var tActsLike2 = tAnon2.ActLike<ISimpeleClassProps>();

           Assert.AreEqual(tActsLike.GetType(), tActsLike2.GetType());

           Assert.AreEqual(tAnon.Prop1, tActsLike.Prop1);
           Assert.AreEqual(tAnon.Prop2, tActsLike.Prop2);
           Assert.AreEqual(tAnon.Prop3, tActsLike.Prop3);

           Assert.AreEqual(tAnon2.Prop1, tActsLike2.Prop1);
           Assert.AreEqual(tAnon2.Prop2, tActsLike2.Prop2);
           Assert.AreEqual(tAnon2.Prop3, tActsLike2.Prop3);

        }


        [Test]
        public void ExpandoPropertyTest()
        {
          
            dynamic tNew = new ExpandoObject();
            tNew.Prop1 = "Test";
            tNew.Prop2 = 42L;
            tNew.Prop3 = Guid.NewGuid();

            var tActsLike = Impromptu.ActLike<ISimpeleClassProps>(tNew);
   



            Assert.AreEqual(tNew.Prop1, tActsLike.Prop1);
            Assert.AreEqual(tNew.Prop2, tActsLike.Prop2);
            Assert.AreEqual(tNew.Prop3, tActsLike.Prop3);
        }


        [Test]
        public void ExpandoMethodsTest()
        {

            dynamic tNew = new ExpandoObject();
            tNew.Action1 = new Action(Assert.Fail);
            tNew.Action2 = new Action<bool>(Assert.IsFalse);
            tNew.Action3 = new Func<string>(()=> "test");

            ISimpeleClassMeth tActsLike = Impromptu.ActLike<ISimpeleClassMeth>(tNew);

			
		
            Assert.Throws<AssertionException>(tActsLike.Action1);
            Assert.Throws<AssertionException>(() => tActsLike.Action2(true));
        
            Assert.AreEqual("test",tActsLike.Action3());
			Console.Write("test");
          
        }



        [Test]
        public void StringPropertyTest()
        {
            var tAnon = "Test 123";
            var tActsLike = tAnon.ActLike<ISimpleStringProperty>();
   

            Assert.AreEqual(tAnon.Length, tActsLike.Length);
        }

        [Test]
        public void StringMethodTest()
        {
            var tAnon = "Test 123";
            var tActsLike = tAnon.ActLike<ISimpleStringMethod>();
      

            Assert.AreEqual(tAnon.StartsWith("Te"),tActsLike.StartsWith("Te"));
        }


        [Test]
        public void OverloadMethodTest()
        {
            var tPoco = new OverloadingMethPoco();
            var tActsLike = tPoco.ActLike<IOverloadingMethod>();

            var tValue = 1;
            Assert.AreEqual("int", tActsLike.Func(tValue));
            Assert.AreEqual("object", tActsLike.Func((object)tValue));
        }

        [Test]
        public void OutMethodTest()
        {
            var tPoco = new MethOutPoco();
            var tActsLike = tPoco.ActLike<IMethodOut>();

            string tResult = String.Empty;

            var tOut = tActsLike.Func(out tResult);

            Assert.AreEqual(true, tOut);
            Assert.AreEqual("success", tResult);
        }

        [Test]
        public void OutMethodTest2()
        {
            var tPoco = new GenericMethOutPoco();
            var tActsLike = tPoco.ActLike<IMethodOut>();

            string tResult = "success";

            var tOut = tActsLike.Func(out tResult);

            Assert.AreEqual(true, tOut);
            Assert.AreEqual(null, tResult);
        }

        [Test]
        public void OutMethodTest3()
        {
            var tPoco = new GenericMethOutPoco();
            var tActsLike = tPoco.ActLike<IMethodOut2>();

            int tResult = 3;

            var tOut = tActsLike.Func(out tResult);

            Assert.AreEqual(true, tOut);
            Assert.AreEqual(0, tResult);
        }

        [Test]
        public void GenericOutMethodTest()
        {
            var tPoco = new GenericMethOutPoco();
            var tActsLike = tPoco.ActLike<IGenericMethodOut>();

            int tResult = 3;

            var tOut = tActsLike.Func(out tResult);

            Assert.AreEqual(true, tOut);
            Assert.AreEqual(0, tResult);

            string tResult2 = "success";

            var tOut2 = tActsLike.Func(out tResult2);

            Assert.AreEqual(true, tOut2);
            Assert.AreEqual(null, tResult2);
        }

        [Test]
        public void RefMethodTest()
        {
            var tPoco = new MethRefPoco();
            var tActsLike = tPoco.ActLike<IMethodRef>();

            int tResult = 1;

            var tOut = tActsLike.Func(ref tResult);

            Assert.AreEqual(true, tOut);
            Assert.AreEqual(3, tResult);

            int tResult2 = 2;

            tOut = tActsLike.Func(ref tResult2);

            Assert.AreEqual(true, tOut);
            Assert.AreEqual(4, tResult2);
        }
	}
}
