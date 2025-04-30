// // Copyright (c) 2024 Engibots. All rights reserved.


namespace engimatrix.ModelObjs.CTT;
public class CttMunicipalityDto
{
    public string cc { get; set; }
    public CttDistrictItem district { get; set; }
    public string name { get; set; }

    public CttMunicipalityDto(string cc, CttDistrictItem district, string name)
    {
        this.cc = cc;
        this.district = district;
        this.name = name;
    }

    public CttMunicipalityDto()
    { }
}
