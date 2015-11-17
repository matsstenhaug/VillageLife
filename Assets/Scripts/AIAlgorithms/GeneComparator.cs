using UnityEngine;
using System;
using System.Collections;

public class GeneComparator : IComparer
{
        int IComparer.Compare(object arg0, object arg1)
        {
            if (((Gene)arg0).mFitness < ((Gene)arg1).mFitness)
            {
                return -1;
            }
            else if (((Gene)arg0).mFitness > ((Gene)arg1).mFitness)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

