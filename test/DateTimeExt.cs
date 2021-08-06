
using System;
using System.Globalization;

namespace test
{
    public static class DateTimeExt
    {
        public static (int y,int m,int d) ToPersian2(this DateTime d)
        {
            PersianCalendar pc = new PersianCalendar();
            return (pc.GetYear(d),pc.GetMonth(d),pc.GetDayOfMonth(d));
        }


        public static string ToPersian(this DateTime d)
        {
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(d)}/{pc.GetMonth(d):00}/{pc.GetDayOfMonth(d):00}";
        }

        public static string ToPersian(this DateTime f,DateTime t)
        {
            var month = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد" ,"مهر","آبان","آذر","دی","بهمن","اسفند"};
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetDayOfMonth(f)}-{pc.GetDayOfMonth(t)} {month[pc.GetMonth(f)-1]} {pc.GetYear(f)}";
        }


        public static DateTime ToGeogrian(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime gdt = new DateTime(dt.Year, dt.Month, dt.Day, pc);
            return gdt;
        }

    }
}
