copy templates\alternet\404-not-found.html site\*.*
call az account clear
call az login
call az storage blob upload-batch --overwrite true --account-name alternetuidocs -s ./site -d $web
call az cdn endpoint purge -g Alternet -n alternet-ui-docs-cdn --profile-name alternet-ui-docs-cdn --content-paths /*

