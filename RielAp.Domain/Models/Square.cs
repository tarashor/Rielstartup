using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    [ComplexType]
    public class Square:IComparable
    {
        public float Value { get; set; }
        public SquareUnit Unit { get; set; }

        public int CompareTo(object obj)
        {
            return 0;
        }
    }

    public enum SquareUnit
    {
        [LocalizationText("SquareUnitSquareMetersLabel")]
        SquareMeters,
        [LocalizationText("SquareUnitAreLabel")]
        Are,
        [LocalizationText("SquareUnitHectareLabel")]
        Hectare
    }
}
