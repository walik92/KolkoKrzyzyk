using System.Collections.Generic;

namespace Common.Core
{
    public class KolkoKrzyzykCore
    {
        private readonly List<int[]> _zwycieskieKombinacje;

        public KolkoKrzyzykCore()
        {
            _zwycieskieKombinacje = new List<int[]>
            {
                new[] {1, 2, 3},
                new[] {4, 5, 6},
                new[] {7, 8, 9},

                new[] {1, 4, 7},
                new[] {2, 5, 8},
                new[] {3, 6, 9},

                new[] {1, 5, 9},
                new[] {3, 5, 7}
            };
        }

        public bool SpradzCzyWygral(char znak, char[] plansza)
        {
            foreach (var kombinacja in _zwycieskieKombinacje)
            {
                if (SprawdzKombinacje(kombinacja, znak, plansza))
                    return true;
            }
            return false;
        }

        public bool SprawdzCzyRemis(char[] plansza)
        {
            foreach (var pole in plansza)
            {
                if (pole == '\0')
                    return false;
            }
            return true;
        }

        private bool SprawdzKombinacje(int[] kombinacja, char znak, char[] plansza)
        {
            foreach (var i in kombinacja)
            {
                if (plansza[i - 1] != znak)
                    return false;
            }
            return true;
        }
    }
}