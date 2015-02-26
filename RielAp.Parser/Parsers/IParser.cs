using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Parser.Parsers
{
    public interface IAnnouncementsParser
    {
        int ParseApartments();
    }

    public interface IPhonesParser
    {
        IDictionary<VashmagazinPhone, int> GetPhones();
    }

    public class VashmagazinPhone:IComparable{
        public string Phone { get; set; }
        public Rubrics Rubric { get; set; }

        public int CompareTo(object obj)
        {
            int res = -1;
            VashmagazinPhone phone = obj as VashmagazinPhone;
            if (phone != null) {
                res = this.Phone.CompareTo(phone.Phone);
                if (res == 0) {
                    if (this.Rubric < phone.Rubric)
                    {
                        res = -1;
                    }
                    else {
                        if (this.Rubric > phone.Rubric)
                        {
                            res = 1;
                        }
                        else {
                            if (this.Rubric == phone.Rubric)
                            {
                                res = 0;
                            }
                        }
                    }
                }
            }
            return res;
        }
    }
}
