import re

# Initialize a list to store the extracted strings
strings = []

# Loop through the file names and extract the strings
for i in range(1, 33):
    filename = f"teethv_#tooth{i:02}.svg"
    with open(filename, "r") as f:
        svg_data = f.read()
        match = re.search(r' d="(.+?)"', svg_data)
        if match:
            strings.append(match.group(1))

# Write the extracted strings to a new file
with open("output_file.xaml", "w") as f:
    for i, string in enumerate(strings):
        f.write(f'<Geometry x:Key="toothTopFillData{i+1}">{string}</Geometry>\n')
