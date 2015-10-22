package Threats.Sickness;

/**
 *
 * @author Zoiids
 */
public abstract class Decease {
    
   // enum NAME{cold, flu};
    private String name;
    private float potency;
    private float resistance;

    public Decease(String name, float potency, float resistance){
        this.name = name;
        this.potency = potency;
        this.resistance = resistance;
    }
    
    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public float getPotency() {
        return potency;
    }

    /**
     * For AI Evolutionary Algorithm
     * @param potency 
     */
    public void setPotency(float potency) {
        this.potency = potency;
    }

    public float getResistance() {
        return resistance;
    }

    /**
     * For AI Evolutionary Algorithm
     * @param resistance 
     */
    public void setResistance(float resistance) {
        this.resistance = resistance;
    }
    
    
    
}
