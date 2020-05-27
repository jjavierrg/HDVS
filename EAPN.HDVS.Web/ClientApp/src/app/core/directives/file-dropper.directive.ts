import { Directive, HostBinding, Output, EventEmitter, HostListener } from '@angular/core';

@Directive({
  selector: '[appFileDropper]',
})
export class FileDropperDirective {
  @HostBinding('class.file-over') fileOver: boolean;
  @Output() filesDropped = new EventEmitter<any>();

  // Dragover listener
  @HostListener('dragover', ['$event']) onDragOver(evt) {
    evt.preventDefault();
    evt.stopPropagation();
    this.fileOver = true;
  }

  // Dragleave listener
  @HostListener('dragleave', ['$event']) public onDragLeave(evt) {
    evt.preventDefault();
    evt.stopPropagation();
    this.fileOver = false;
  }

  // Drop listener
  @HostListener('drop', ['$event']) public ondrop(evt) {
    evt.preventDefault();
    evt.stopPropagation();
    this.fileOver = false;
    const files = evt.dataTransfer.files;
    if (files.length > 0) {
      this.filesDropped.emit(files);
    }
  }
}
