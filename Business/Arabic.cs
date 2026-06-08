namespace Business
{
    public static class Arabic
    {
        public static string ConvertNumerals(this string input)
        {
            //if (new string[] { "ar-lb", "ar-SA" }
            //      .Contains(Thread.CurrentThread.CurrentCulture.Name))
            //{
            return input.Replace('0', '\u0660')
                    .Replace('1', '\u0661')
                    .Replace('2', '\u0662')
                    .Replace('3', '\u0663')
                    .Replace('4', '\u0664')
                    .Replace('5', '\u0665')
                    .Replace('6', '\u0666')
                    .Replace('7', '\u0667')
                    .Replace('8', '\u0668')
                    .Replace('9', '\u0669')
                    .Replace('.', '\u066B');

            //}
            //else return input;
        }
        public static string ConvertLogic(this bool input)
        {
            return input == true ? "نعم" : "لا";
        }
    }
}
