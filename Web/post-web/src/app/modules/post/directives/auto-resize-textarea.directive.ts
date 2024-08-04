import { Directive, ElementRef, HostListener, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appAutoResizeTextarea]'
})
export class AutoResizeTextareaDirective {

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  @HostListener('input') onInput() {
    this.resize()
  }

  private resize(): void {
    const textArea = this.el.nativeElement as HTMLTextAreaElement

    this.renderer.setStyle(textArea, 'height', 'auto')
    this.renderer.setStyle(textArea, 'height', `${textArea.scrollHeight}px`)
  }
}
