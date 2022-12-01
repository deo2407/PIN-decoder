using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        public int valid(string egn)
        {
            int ostatyk;
            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                int num = egn[i] - '0';
                sum += num * weight[i + 1];
            }

            if (sum % 11 == 0 || sum % 11 == 10)
                ostatyk = 0;
            else
                ostatyk = sum % 11;

            return ostatyk;
        }

        public void updateForm(bool isValid, int year, int month, int day, string gender, string city)
        {
            boxYear.Text = year.ToString();
            boxMonth.Text = monthDict[month];
            boxDay.Text = day.ToString();
            boxPlace.Text = city;
            if (isValid)
                boxDostovernost.Text = "Valid";
            else
                boxDostovernost.Text = "Invalid";
            boxPol.Text = gender;
        }

        static void formatRegions(string regions, Dictionary<HashSet<int>, string> dict)
        {
            List<string> data = regions.Split(' ').ToList();

            for (int i = 0; i < data.Count; i++)
            {
                string city = data[i];
                i++;

                string range = data[i];
                int[] rangeNums = range.Split('-').Select(n => Convert.ToInt32(n)).ToArray();
                rangeNums[1] -= 1;
                HashSet<int> hash = new HashSet<int>(rangeNums);

                dict.Add(hash, city);
            }
        }

        string unformatedRegions = "Blagoevgrad 0-43 Burgas 43-93 Varna 93-139 Veliko-Tarnovo 139-169 Vidin 169-183 Vratsa 183-217 Gabrovo 217-233 Kardzhali 233-281 Kyustendil 281-301 Lovech 301-319 Montana 319-341 Pazardzhik 341-377 Pernik 377-395 Pleven 395-435 Plovdiv 435-501 Razgrad 501-527 Ruse 527-555 Silistra 555-575 Sliven 575-601 Smolyan 601-623 Sofia-city 623-721 Sofia-district 721-751 Stara-Zagora 751-789 Dobrich 789-821 Targovishte 821-843 Haskovo 843-871 Shumen 871-903 Yambol 903-925 Other 925-999";

        Dictionary<HashSet<int>, string> regionsDict = new Dictionary<HashSet<int>, string>();

        Dictionary<int, int> weight = new Dictionary<int, int>
        {
            {1, 2},
            {2, 4},
            {3, 8},
            {4, 5},
            {5, 10},
            {6, 9},
            {7, 7},
            {8, 3},
            {9, 6}
        };

        Dictionary<int, string> monthDict = new Dictionary<int, string>
        {
            {1, "January" },
            {2, "February" },
            {3, "March" },
            {4, "April" },
            {5, "May" },
            {6, "June" },
            {7, "July" },
            {8, "August" },
            {9, "September" },
            {10, "October" },
            {11, "November" },
            {12, "December" }
        };

        private void showResult_Click(object sender, EventArgs e)
        {
            string EGN = boxEGN.Text;
            bool isNumber = long.TryParse(EGN, out long numericValue);
            if (EGN.Count() != 10 || !isNumber)
            {
                MessageBox.Show("Неправилно въведено ЕГН!");
                Application.Restart();
            }
                
            else
            {
                int year = int.Parse(EGN.Substring(0, 2));
                int month = int.Parse(EGN.Substring(2, 2));
                int day = int.Parse(EGN.Substring(4, 2));
                int cityNum = int.Parse(EGN.Substring(6, 3));
                string city = "";
                string gender;
                int lastDigit = EGN[EGN.Count() - 1] - '0';
                int remainder;
                bool isValid = true;

                // Validating
                remainder = valid(EGN);
                if (remainder != lastDigit)
                    isValid = false;

                if (!isValid)
                {
                    MessageBox.Show("Невалидно ЕГН!");
                    Application.Restart();
                }
                    
                else
                {
                    // Formating the year
                    year += 1900;
                    if (month - 40 > 0)
                        year += 100;

                    // Formating the month
                    if (month - 40 > 0)
                        month -= 40;

                    // Formating sex
                    if (cityNum % 2 == 0)
                        gender = "Male";
                    else
                        gender = "Female";

                    // Formating the birth place
                    formatRegions(unformatedRegions, regionsDict);
                    foreach (var value in regionsDict.Keys)
                    {
                        int[] nums = new int[value.Count];
                        value.CopyTo(nums);
                        if (cityNum >= nums[0] && cityNum <= nums[1])
                        {
                            city = regionsDict[value];
                            break;
                        }
                    }

                    // Updating the interface
                    updateForm(isValid, year, month, day, gender, city);
                }
            }
        }
    }
}
