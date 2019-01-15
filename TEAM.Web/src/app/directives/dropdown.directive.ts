import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appDropdown]'
})
export class DropdownDirective {

  constructor(private elementRef: ElementRef) { }

  private dropdownParentEl = this.elementRef.nativeElement.closest('.dropdown');

  @HostListener('click') toggleOpen() {
    this.dropdownParentEl.querySelector(".dropdown-menu").classList.toggle('show');
  }

  @HostListener('document:click', ['$event']) clickout(event) {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.dropdownParentEl.querySelector(".dropdown-menu").classList.remove('show');
    }
  }
}
