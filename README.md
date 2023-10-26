# My C# XML Serialize/Deserialize Note
Here notes are for `System.Xml.Serialization.XmlSerializer`.</br>
* The class which you want to serialize/deserialize to xml need a default constructor, which is no any arguments constructor.
* The properties which you want to serialize/deserialize must have public setter and getter.
	* You can finely adjust property to be attribute or xml node via `[XmlAttribute]`, `[XmlElement]`. And you also change name showing xml with the attribute, e.g. `[XmlAttribute("Test")]` let the attribute show `Test` instead of its property name.