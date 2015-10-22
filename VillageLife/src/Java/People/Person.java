package People;

import Threats.Sickness.Decease;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author Zoiids
 */
public class Person {
    
    /**
     * Attributes:
     * 
     * Health
     * Gender
     * Age
     * Strength
     * Sexual preference
     * Immunities
     * Weaknesses
     * NextGen mutation factor
     */
    
    private float health;
    private GENDER g;
    private int age;
    private float strength;
    private SEXPREF sp;
    private float NGMF = 1f;

    
    private ArrayList<Decease> immunities;
    private ArrayList<Decease> weaknesses;
    
    public enum GENDER{ male, female };
    public enum SEXPREF{ hetero, homo, bi, a};
    
    public Person(float hp, GENDER g, int age, float str, SEXPREF sp, ArrayList<Decease> imm, ArrayList<Decease> weak){
        this.health = hp;
        this.g = g;
        this.age = age;
        this.strength = str;
        this.sp = sp;
        this.immunities = imm;
        this.weaknesses = weak;
    }

    public float getHealth() {
        return health;
    }

    public GENDER getG() {
        return g;
    }

    public int getAge() {
        return age;
    }

    public float getStrength() {
        return strength;
    }

    public SEXPREF getSp() {
        return sp;
    }

    public ArrayList<Decease> getImmunities() {
        return immunities;
    }

    public ArrayList<Decease> getWeaknesses() {
        return weaknesses;
    }
    
    public float getNGMF() {
        return NGMF;
    }

    public void setNGMF(float NGMF) {
        this.NGMF = NGMF;
    }
    
}
