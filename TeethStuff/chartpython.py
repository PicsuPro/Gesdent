import re

# Open the input SVG file and read its contents
with open('chart.xaml', 'r') as f:
    svg_data = f.read()
    fill_svg_data = svg_data


# Define the pattern to match the tooth paths
fill_pattern = r'<Path Name="toothFill(\d+)" Data="(.+)"\/>'

# Find all the tooth paths and their data
fill_tooth_paths = re.findall(fill_pattern, fill_svg_data)

# Define the output template for the tooth geometry elements
fill_geometry_template = '<Geometry x:Key="toothFillData{0}">{1}</Geometry>'

# Combine the data for each tooth into a geometry element
fill_tooth_geometries = [fill_geometry_template.format(fill_tooth_num, fill_tooth_data) for fill_tooth_num, fill_tooth_data in fill_tooth_paths]

# Sort the geometries by tooth number
fill_tooth_geometries.sort(key=lambda x: int(re.search(r'toothFillData(\d+)', x).group(1)))

# Join the geometry elements into a single string
fill_tooth_geometries_str = '\n'.join(fill_tooth_geometries)

# Define the pattern to match the tooth paths
pattern = r'<Path Name="tooth(\d+)" Data="(.+)"\/>'

# Find all the tooth paths and their data
tooth_paths = re.findall(pattern, svg_data)

# Define the output template for the tooth geometry elements
geometry_template = '<Geometry x:Key="toothData{0}">{1}</Geometry>'

# Combine the data for each tooth into a geometry element
tooth_geometries = [geometry_template.format(tooth_num, tooth_data) for tooth_num, tooth_data in tooth_paths]

# Sort the geometries by tooth number
tooth_geometries.sort(key=lambda x: int(re.search(r'toothData(\d+)', x).group(1)))

# Join the geometry elements into a single string
tooth_geometries_str = '\n'.join(tooth_geometries)

# Replace the tooth paths with the combined geometry elements in the SVG data
svg_data = re.sub(pattern, '', svg_data)
fill_svg_data = re.sub(fill_pattern, '', fill_svg_data)
svg_data = svg_data.replace('<Canvas Name="svg5" Width="180" Height="100">', '<Canvas Width="180" Height="100">\n' + tooth_geometries_str + '\n</Canvas>\n')
fill_svg_data = fill_svg_data.replace('<Canvas Name="svg5" Width="180" Height="100">', '<Canvas Width="180" Height="100">\n' + fill_tooth_geometries_str + '\n</Canvas>\n')

# Write the modified SVG data to the output file

with open('output.xaml', 'w') as f:
    f.write(fill_svg_data)
    f.write(svg_data)
    
