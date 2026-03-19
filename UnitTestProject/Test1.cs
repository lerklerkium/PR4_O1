using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int res = 2 + 2;
            Assert.AreEqual(res, 4);
            Assert.AreNotEqual(res, 5);
            Assert.IsFalse(res > 5);
            Assert.IsTrue(res < 5);
        }

        private static object _page;
        private static Type _pageType;

        /// <summary>
        /// .sta
        /// </summary>
        private static void RunOnSta(Action action)
        {
            Exception caught = null;
            var thread = new Thread(() =>
            {
                try { action(); }
                catch (Exception ex) { caught = ex; }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            if (caught != null)
                throw caught.InnerException ?? caught;
        }

        /// <summary>
        /// получение текстовых полей по имени
        /// </summary>
        private static TextBox GetTextBox(string name)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

            var field = _pageType.GetField(name, flags);
            if (field != null)
            {
                var tb = field.GetValue(_page) as TextBox;
                Assert.IsNotNull(tb, $"Поле '{name}' равно null или не TextBox.");
                return tb;
            }

            Assert.Fail($"Поле или свойство '{name}' не найдено в {_pageType.Name}.");
            return null;
        }

        /// <summary>
        /// вызов страницы
        /// </summary>
        private static void InvokeMethod(string methodName)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
            var method = _pageType.GetMethod(methodName, flags);
            Assert.IsNotNull(method, $"Метод '{methodName}' не найден в {_pageType.Name}.");
            method.Invoke(_page, new object[] { null, new RoutedEventArgs() });
        }

        /// <summary>
        /// задание значений
        /// </summary>
        private static void SetText(string fieldName, string value)
        {
            GetTextBox(fieldName).Text = value;
        }

        /// <summary>
        /// получение результата
        /// </summary>
        private static string GetResult()
        {
            return GetTextBox("TbResult").Text;
        }

        /// <summary>
        /// очистка
        /// </summary>
        private static void ClearResult()
        {
            GetTextBox("TbResult").Text = string.Empty;
        }

        [TestMethod]
        public void TestCalculateClick1()
        {
            RunOnSta(() =>
            {
                var assembly = Assembly.GetAssembly(typeof(PR4_O.Pages.Task1Page));
                Assert.IsNotNull(assembly, "Сборка не найдена.");

                _page = new PR4_O.Pages.Task1Page();
                _pageType = _page.GetType();

                SetText("TbX", "2");
                SetText("TbY", "1");
                SetText("TbZ", "1");
                ClearResult();
                InvokeMethod("Calculate_Click1");

                double inner = 1.0 + Math.Cbrt(1.0);
                double den = 1.0 * (Math.Sin(1) * Math.Sin(1) + Math.Tan(1));
                double expected = Math.Pow(inner, 0.25) / den;
                string result1 = GetResult();

                Assert.AreNotEqual(string.Empty, result1);
                Assert.AreEqual(expected.ToString("G10"), result1);
                Assert.IsTrue(double.TryParse(result1, out double val1));
                Assert.IsTrue(val1 > 0);

                SetText("TbX", "3");
                SetText("TbY", "3");
                ClearResult();
                InvokeMethod("Calculate_Click1");

                string result2 = GetResult();
                Assert.AreEqual(string.Empty, result2);

                SetText("TbX", "2");
                SetText("TbY", "1");
                SetText("TbZ", "0");
                ClearResult();
                InvokeMethod("Calculate_Click1");

                string result3 = GetResult();
                Assert.AreEqual(string.Empty, result3);

                SetText("TbX", "0");
                SetText("TbY", "0");
                SetText("TbZ", "1");
                ClearResult();
                InvokeMethod("Calculate_Click1");

                string result4 = GetResult();
                Assert.AreEqual(string.Empty, result4);

                SetText("TbX", "2");
                SetText("TbY", "-1");
                SetText("TbZ", "1");
                ClearResult();
                InvokeMethod("Calculate_Click1");

                string result5 = GetResult();
                Assert.AreNotEqual(string.Empty, result5);
                Assert.IsTrue(double.TryParse(result5, out double val5));
                Assert.AreEqual(0.0, val5, 1e-15);

                SetText("TbX", "abc");
                ClearResult();
                InvokeMethod("Calculate_Click1");

                string result6 = GetResult();
                Assert.AreEqual(string.Empty, result6);

                SetText("TbX", "");
                ClearResult();
                InvokeMethod("Calculate_Click1");

                string result7 = GetResult();
                Assert.AreEqual(string.Empty, result7);
            });
        }

        [TestMethod]
        public void TestCalculateClick2()
        {
            RunOnSta(() =>
            {
                var assembly = Assembly.GetAssembly(typeof(PR4_O.Pages.Task2Page));
                Assert.IsNotNull(assembly, "Сборка Mehto не найдена.");

                _page = new PR4_O.Pages.Task2Page();
                _pageType = _page.GetType();

                SetText("TbX", "1");
                SetText("TbY", "1");
                ClearResult();
                InvokeMethod("Calculate_Click2");

                string result1 = GetResult();
                Assert.AreNotEqual(string.Empty, result1);
                Assert.IsTrue(double.TryParse(result1, out double val1));
                Assert.IsFalse(double.IsNaN(val1));
                Assert.IsFalse(double.IsInfinity(val1));

                SetText("TbX", "5");
                SetText("TbY", "2");
                ClearResult();
                InvokeMethod("Calculate_Click2");

                string result2 = GetResult();
                Assert.AreNotEqual(string.Empty, result2);
                Assert.IsTrue(double.TryParse(result2, out double val2));
                Assert.IsFalse(double.IsNaN(val2));

                SetText("TbX", "1");
                SetText("TbY", "5");
                ClearResult();
                InvokeMethod("Calculate_Click2");

                string result3 = GetResult();
                Assert.AreNotEqual(string.Empty, result3);
                Assert.IsTrue(double.TryParse(result3, out double val3));
                Assert.IsFalse(double.IsNaN(val3));

                SetText("TbX", "abc");
                SetText("TbY", "1");
                ClearResult();
                InvokeMethod("Calculate_Click2");

                string result4 = GetResult();
                Assert.AreEqual(string.Empty, result4);

                SetText("TbX", "1");
                SetText("TbY", "xyz");
                ClearResult();
                InvokeMethod("Calculate_Click2");

                string result5 = GetResult();
                Assert.AreEqual(string.Empty, result5);

                SetText("TbX", "");
                SetText("TbY", "");
                ClearResult();
                InvokeMethod("Calculate_Click2");

                string result6 = GetResult();
                Assert.AreEqual(string.Empty, result6);

                SetText("TbX", "0");
                SetText("TbY", "0");
                ClearResult();
                InvokeMethod("Calculate_Click2");

                string result7 = GetResult();
                Assert.AreNotEqual(string.Empty, result7);
                Assert.IsTrue(double.TryParse(result7, out double val7));
            });
        }

        [TestMethod]
        public void TestCalculateClick3()
        {
            RunOnSta(() =>
            {
                var assembly = Assembly.GetAssembly(typeof(PR4_O.Pages.Task3Page));
                Assert.IsNotNull(assembly, "Сборка Mehto не найдена.");

                _page = new PR4_O.Pages.Task3Page();
                _pageType = _page.GetType();

                SetText("TbB", "0");
                SetText("TbX0", "1");
                SetText("TbXk", "3");
                SetText("TbDx", "1");
                ClearResult();
                InvokeMethod("Calculate_Click3");

                string result1 = GetResult();
                Assert.AreNotEqual(string.Empty, result1);
                Assert.IsTrue(result1.Length > 0);
                Assert.IsFalse(string.IsNullOrWhiteSpace(result1));

                SetText("TbB", "0");
                SetText("TbX0", "1");
                SetText("TbXk", "3");
                SetText("TbDx", "-1");
                ClearResult();
                InvokeMethod("Calculate_Click3");

                string result2 = GetResult();
                Assert.AreEqual(string.Empty, result2);

                SetText("TbDx", "0");
                ClearResult();
                InvokeMethod("Calculate_Click3");

                string result3 = GetResult();
                Assert.AreEqual(string.Empty, result3);

                SetText("TbX0", "5");
                SetText("TbXk", "3");
                SetText("TbDx", "1");
                ClearResult();
                InvokeMethod("Calculate_Click3");

                string result4 = GetResult();
                Assert.AreEqual(string.Empty, result4);

                SetText("TbX0", "3");
                SetText("TbXk", "3");
                ClearResult();
                InvokeMethod("Calculate_Click3");

                string result5 = GetResult();
                Assert.AreEqual(string.Empty, result5);

                SetText("TbB", "2");
                SetText("TbX0", "1.5");
                SetText("TbXk", "2.5");
                SetText("TbDx", "0.5");
                ClearResult();
                InvokeMethod("Calculate_Click3");

                string result6 = GetResult();
                Assert.AreNotEqual(string.Empty, result6);
                Assert.IsTrue(result6.Contains("не опр."));

                SetText("TbB", "abc");
                ClearResult();
                InvokeMethod("Calculate_Click3");

                string result7 = GetResult();
                Assert.AreEqual(string.Empty, result7);

                SetText("TbB", "0");
                SetText("TbX0", "1");
                SetText("TbXk", "3");
                SetText("TbDx", "qqq");
                ClearResult();
                InvokeMethod("Calculate_Click3");

                string result8 = GetResult();
                Assert.AreEqual(string.Empty, result8);
            });
        }
    }
}