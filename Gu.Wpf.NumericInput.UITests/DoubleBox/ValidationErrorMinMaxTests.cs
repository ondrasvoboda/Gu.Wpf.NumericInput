namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class ValidationErrorMinMaxTests : ValidationTestsBase
    {
        public static readonly MinMaxData[] MinMaxSource =
            {
                new MinMaxData("-2", "-1", "", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.'"),
                new MinMaxData("-2.1", "-1.1", "", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.1.'"),
                new MinMaxData("-2", "-1", "1", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.'"),
                new MinMaxData("-2.1", "-1.1", "1.1", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.1.'"),
                new MinMaxData("2", "", "1", "1", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.'"),
                new MinMaxData("2.1", "", "1.1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.1.'"),
                new MinMaxData("2", "-1", "1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.'"),
                new MinMaxData("2.1", "-1.1", "1.1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.1.'"),
            };

        [SetUp]
        public void SetUp()
        {
            this.ViewModelValueBox.Text = "0";
            this.CultureBox.Select("en-US");
            this.CanValueBeNullBox.Checked = false;

            this.AllowLeadingWhiteBox.Checked = true;
            this.AllowTrailingWhiteBox.Checked = true;
            this.AllowLeadingSignBox.Checked = true;
            this.AllowDecimalPointBox.Checked = true;
            this.AllowThousandsBox.Checked = false;
            this.AllowExponentBox.Checked = true;

            this.MinBox.Text = "";
            this.MaxBox.Text = "";
            this.LoseFocusButton.Click();
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void LostFocus(MinMaxData data)
        {
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = data.StartValue;
            this.LoseFocusButton.Click();
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;

            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, this.Window.Get<Label>("LostFocusValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void LostFocusValidateOnPropertyChanged(MinMaxData data)
        {
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = data.StartValue;
            this.LoseFocusButton.Click();
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, this.Window.Get<Label>("LostFocusValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void PropertyChanged(MinMaxData data)
        {
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = data.StartValue;
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, this.Window.Get<Label>("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCase("3", "ValidationError.IsGreaterThanValidationResult 'V�nligen ange ett v�rde mindre �n eller lika med 2,2.'")]
        [TestCase("-3", "ValidationError.IsLessThanValidationResult 'V�nligen ange ett v�rde st�rre �n eller lika med -2,1.'")]
        public void PropertyChangedSwedish(string value, string infoMessage)
        {
            this.CultureBox.Select("sv-SE");
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = "1";
            this.MinBox.Text = "-2.1";
            this.MaxBox.Text = "2.2";
            doubleBox.Text = value;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(MinMaxData.GetErrorMessage(infoMessage), this.Window.Get<Label>("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual("1", this.ViewModelValueBox.Text);
        }

        [TestCase("3", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 2.2.'")]
        [TestCase("-3", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -2.1.'")]
        public void PropertyChangedWhenNotLocalized(string value, string infoMessage)
        {
            this.CultureBox.Select("ja-JP");
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = "1";
            this.MinBox.Text = "-2.1";
            this.MaxBox.Text = "2.2";
            doubleBox.Text = value;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(MinMaxData.GetErrorMessage(infoMessage), this.Window.Get<Label>("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual("1", this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void PropertyChangedWhenNull(MinMaxData data)
        {
            this.CanValueBeNullBox.Checked = true;
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = "";
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, this.Window.Get<Label>("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("", this.ViewModelValueBox.Text);
        }

        public class MinMaxData
        {
            public readonly string Text;
            public readonly string Min;
            public readonly string Max;
            public readonly string Expected;
            public readonly string ExpectedInfoMessage;

            public MinMaxData(string text, string min, string max, string expected, string expectedInfoMessage)
            {
                this.Text = text;
                this.Min = min;
                this.Max = max;
                this.Expected = expected;
                this.ExpectedInfoMessage = expectedInfoMessage;
            }

            public string ErrorMessage => GetErrorMessage(this.ExpectedInfoMessage);

            public string StartValue => string.IsNullOrEmpty(this.Min)
                                            ? this.Max
                                            : this.Min;

            public static string GetErrorMessage(string infoMessage)
            {
                return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
            }

            public override string ToString() => $"Text: {this.Text}, Min: {this.Min}, Max: {this.Max}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedInfoMessage}";
        }
    }
}