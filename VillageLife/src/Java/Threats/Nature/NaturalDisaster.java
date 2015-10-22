package Threats.Nature;

/**
 *
 * @author Zoiids
 */
public abstract class NaturalDisaster {
    
    private String name; 
    private float lethality;
    private float probability;
    
    public NaturalDisaster(String name, float lethatlity, float probability){
        this.name = name;
        this.lethality = lethatlity;
        this.probability = probability;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public float getLethality() {
        return lethality;
    }

    public void setLethality(float lethality) {
        this.lethality = lethality;
    }

    public float getProbability() {
        return probability;
    }

    public void setProbability(float probability) {
        this.probability = probability;
    }
    
    
}
