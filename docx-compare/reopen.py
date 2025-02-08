import os
import sys
import time
import win32com.client
from pathlib import Path
import shutil

word_app = win32com.client.Dispatch("Word.Application")
word_app.Visible = True

doc_path = Path(r"c:\tmp\Dok1.docx") if len(sys.argv) < 2 else Path(sys.argv[1])
tmp_path = Path(r"c:\tmp\Dok1-tmp.docx")
marker_path = Path(r"c:\tmp\Dok1-tmp.marker")

try:
    while True:
        # Force stop of previous instance
        if marker_path.exists():
            marker_path.write_text('stop')
            i = 0
            while marker_path.exists() and i < 10:
                time.sleep(0.5)
                i += 1
            time.sleep(0.5)
        marker_path.write_text('start')

        # Keep time stamp of the document
        time_stamp = doc_path.stat().st_mtime

        # Copy the document to a temporary file
        try:
            tmp_path.unlink()
        except: pass
        shutil.copy(doc_path, tmp_path)

        # Open the document
        doc = word_app.Documents.Open(str(tmp_path), ReadOnly=True)
        while True:
            # Check if the document is still open
            try:
                doc.ActiveWindow
            except Exception as ex:
                if (ex.hresult == -2147417848):
                    try:
                        doc.Close(SaveChanges=False)
                    except: pass
                    time.sleep(0.5)
                    exit()
                elif (ex.hresult == -2147418111):
                    pass
                else:
                    raise
            # Check if the document has been modified, reopen if yes
            if doc_path.stat().st_mtime != time_stamp:
                break
            # Check if exit was requested over marker file
            if marker_path.read_text() == 'stop':
                doc.Close(SaveChanges=False)
                time.sleep(0.5)
                exit()
            # Sleep
            time.sleep(0.5)
        # Close the document
        doc.Close(SaveChanges=False)
        time.sleep(0.5)

# Always remove the marker file
finally:
    try:
        marker_path.unlink()
    except: pass
