using System;

// This class will be an interface for the KEditorManager to draw different forms.
public interface IKForm {
	//KEditorManager manager { get; set; }

	void FormGUI(KEditorManager manager);

	void FormUpdate(KEditorManager manager);

	void OnDestroy();
}