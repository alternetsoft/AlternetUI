import sysconfig

# Get all configuration variables as a dictionary
config_vars = sysconfig.get_config_vars()

# Print each variable name and its corresponding value
for name, value in config_vars.items():
    print(f"{name}: {value}")
