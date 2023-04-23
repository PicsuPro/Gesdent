import re

# Define a regular expression to match Path elements
path_regex = r'<Path.*?Name="(.*?)".*?Data="(.*?)".*?/>'

# Define a natural sorting function
def natural_sort_key(s):
    # split the string into chunks of digits and non-digits
    # e.g. "Path123" becomes ["Path", "123"]
    return [int(c) if c.isdigit() else c for c in re.split(r'(\d+)', s)]

with open('chart.xaml', 'r') as f:
    xaml = f.read()

# Find all Path elements in the XAML and extract their Name and Data attributes
paths = re.findall(path_regex, xaml, re.DOTALL)

# Sort the paths by their name using natural sorting
paths = sorted(paths, key=lambda path: natural_sort_key(path[0]))

# Create a string to store the resulting Geometry elements
geometry_string = ''

# Loop through each Path element and convert it to a Geometry element
for path in paths:
    geometry_string += f'<Geometry x:Key="{path[0]}">{path[1]}</Geometry>\n'

# Write the resulting Geometry elements to a file
with open('geometries.xaml', 'w') as f:
    f.write(f'<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"\n'
            f'                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">\n'
            f'{geometry_string}'
            f'</ResourceDictionary>')
