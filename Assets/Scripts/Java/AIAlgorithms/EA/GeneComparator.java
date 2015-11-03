/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package AIAlgorithms.EA;

import java.util.Comparator;

/**
 *
 * @author Zoiids
 */

public class GeneComparator {
    
    public int compare(Gene arg0, Gene arg1){
        if(arg0.mFitness < arg1.mFitness){
            return -1;
        }else if( arg0.mFitness > arg1.mFitness){
            return 1;
        }else{
            return 0;
        }
    }
    
}
