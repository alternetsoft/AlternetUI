import sysconfig
import xml.etree.ElementTree as ET
import xml.dom.minidom

# Create XML root element
root = ET.Element("SchemePaths")

# Retrieve paths for the default scheme
scheme_paths = sysconfig.get_paths(sysconfig.get_default_scheme())

# Add path entries to XML
for path_name, location in scheme_paths.items():
    entry = ET.SubElement(root, "Path", name=path_name)
    entry.text = location

# Convert XML to a formatted string and print
xml_data = ET.tostring(root, encoding="utf-8").decode("utf-8")
formatted_xml = xml.dom.minidom.parseString(xml_data).toprettyxml(indent="  ")
print(formatted_xml)
