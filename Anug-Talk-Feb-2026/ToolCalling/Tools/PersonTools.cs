using ToolCalling.Models;

namespace ToolCalling.Tools;

//Information Tool
public class PersonTools
{
    public PersonInfo[] GetPersons()
    {
        return GetData();
    }

    public PersonInfoDetails? GetPerson(int personId)
    {
        return personId switch
        {
            1 => new PersonInfoDetails("Blue", "Denmark"), //Ben
            2 => new PersonInfoDetails("Green", "Canada"), //Susan
            3 => new PersonInfoDetails("Red", "Sweden"), //Jenny
            _ => null
        };
    }

    private static PersonInfo[] GetData()
    {
        return
        [
            new PersonInfo(1, "Ben"),
            new PersonInfo(2, "Susan"),
            new PersonInfo(3, "Jenny")
        ];
    }
}
