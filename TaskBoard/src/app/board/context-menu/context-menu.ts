import { Component, ElementRef, HostListener } from '@angular/core';

@Component({
  selector: 'app-context-menu',
  imports: [],
  templateUrl: './context-menu.html',
  styleUrl: './context-menu.css',
})
export class ContextMenu {
  constructor(private elementRef: ElementRef) {}

  isMenuOpen = false;
  toggle(){
    this.isMenuOpen = !this.isMenuOpen;
  }

  @HostListener('document:click', ['$event'])
  OnDocumentClick(event: MouseEvent){
    const clickedInside = this.elementRef.nativeElement.contains(event.target);
    if (!clickedInside){
      this.isMenuOpen = false;
    }
  }
}
