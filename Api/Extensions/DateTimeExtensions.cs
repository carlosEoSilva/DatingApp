namespace Api.Extensions
{
    public static class DateTimeExtensions
    {
        //- dob = date of birth
        public static int CalculateAge(this DateTime dob)
        {
            // var today= DateOnly.FromDateTime(DateTime.UtcNow);
            // var age= today.Year - dob.Year;
            // if(dob.Year > today.AddYears(-age)) 
            //     age--;
            // return age;

            var today = DateTime.Today;

            var age = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-age)) age--;

            return age;
        }
        
    }
}