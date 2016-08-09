Following commands can be used in order to create new dependencies (ZIP) which can then be uploaded manually on the FTP server.

find . -type f > allfiles.txt
while read line; do filenam=`echo $line | sed -e 's/\//-/g' | sed -e 's/.-//'`; zip "$filenam.zip" "$line"; done < allfiles.txt

ls *.zip > zipfiles.txt
while read line; do siz=`du -b "$line" | cut -f 1`; sm=`sum "$line" | awk '{print $1}'`; echo $line,$siz,$sm; done < zipfiles.txt > packages.lst

rm allfiles.txt zipfiles.txt


Following GIT command is used to remove a directory completey from the git repository from all possible locations so as to reduce the size of the repo. (CAUTION)
git filter-branch --prune-empty --subdirectory-filter \ SourceCode master