[System.Serializable]
public class CreatureData
{
    public Creature[] creature;
}

[System.Serializable]
public class Creature
{
    public int id;
    public string name;
    public string filial;
    public string tipo;
    public string descricao;
}