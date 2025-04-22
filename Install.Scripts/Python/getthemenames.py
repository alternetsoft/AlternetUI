import sysconfig

# Get all available scheme names
scheme_names = sysconfig.get_scheme_names()

# Print each scheme name
for scheme in scheme_names:
    print(scheme)

# nt
# nt_user
# nt_venv
# osx_framework_user
# posix_home
# posix_prefix
# posix_user
# posix_venv
# venv
