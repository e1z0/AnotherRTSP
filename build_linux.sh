#!/bin/bash
# Generate Linux AppImage Bundle with Wine for AnotherRTSP project
# (c) 2024 e1z0

# Variables
DIST_DIR=./bin      # Bin directory, where the compiled program files lies
WINE_USR=./wine/usr # download from https://sourceforge.net/projects/wine/files/Slackware%20Packages/9.12/i686/ (Extract tar -xf)
export ARCH=i686
export APP_NAME="AnotherRTSP"
export WINEPREFIX="$HOME/${APP_NAME}_wineprefix"
export WINEARCH=win32
export APP_DIR="${APP_NAME}.AppDir"
export ICON_PATH="icons/256.png"
export DESKTOP_FILE="${APP_NAME}.desktop"
export APPIMAGE_TOOL=appimagetool-x86_64.AppImage

# Install .NET using Winetricks
winetricks win7 dotnet40

# Create AppImage directory structure
mkdir -p "${APP_DIR}/usr/bin"
mkdir -p "${APP_DIR}/usr/lib"
mkdir -p "${APP_DIR}/usr/share/applications"
mkdir -p "${APP_DIR}/usr/share/icons/hicolor/256x256/apps"

# copy application to wine prefix
cp -r ${DIST_DIR} $WINEPREFIX/drive_c/AnotherRTSP

# Copy Wine prefix to AppImage directory
cp -r "$WINEPREFIX" "${APP_DIR}/usr/"
# Copy Wine binaries to AppImage directory
cp -r ${WINE_USR} "${APP_DIR}/"


# Create launcher script
cat <<EOF > "${APP_DIR}/AppRun"
#!/bin/bash
export WINEPREFIXOLD=\$(dirname "\$0")/usr/${APP_NAME}_wineprefix
if ! [[ -d "\$HOME/.${APP_NAME}_wine" ]]; then
cp -r \$WINEPREFIXOLD \$HOME/.${APP_NAME}_wine
fi
export WINEPREFIX=\$HOME/.${APP_NAME}_wine
export PATH=\$PATH:\$(dirname "\$0")/usr/bin
pushd \$WINEPREFIX/drive_c/AnotherRTSP
wine \$WINEPREFIXOLD/drive_c/AnotherRTSP/AnotherRTSP.exe
popd
EOF


chmod +x "${APP_DIR}/AppRun"

# Create desktop entry
cat <<EOF > "${APP_DIR}/${DESKTOP_FILE}"
[Desktop Entry]
Name=${APP_NAME}
Exec=AppRun
Icon=${APP_NAME}
Type=Application
Categories=Network;
EOF

# Add icon
cp "${ICON_PATH}" "${APP_DIR}/usr/share/icons/hicolor/256x256/apps/"
cp "${ICON_PATH}" "${APP_DIR}/${APP_NAME}.png"

# Download appimagetool if not already downloaded
if [ ! -f "${APPIMAGE_TOOL}" ]; then
    wget "https://github.com/AppImage/AppImageKit/releases/download/continuous/appimagetool-x86_64.AppImage" -O ${APPIMAGE_TOOL}
    chmod +x "${APPIMAGE_TOOL}"
fi

# Create the AppImage
./${APPIMAGE_TOOL} "${APP_DIR}"

# Clean up
rm -rf "${APP_DIR}"
echo "AppImage for ${APP_NAME} created successfully!"
