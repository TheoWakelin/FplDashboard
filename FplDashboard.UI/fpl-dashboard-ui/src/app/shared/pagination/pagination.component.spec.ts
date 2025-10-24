import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PaginationComponent } from './pagination.component';

describe('PaginationComponent', () => {
  let component: PaginationComponent;
  let fixture: ComponentFixture<PaginationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaginationComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(PaginationComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should emit page change event', () => {
    spyOn(component.pageChange, 'emit');
    component.currentPage = 1;
    component.totalPages = 5;
    component.goToPage(2);
    expect(component.pageChange.emit).toHaveBeenCalledWith(2);
  });

  it('should go to previous page', () => {
    spyOn(component.pageChange, 'emit');
    component.currentPage = 3;
    component.totalPages = 5;
    component.prevPage();
    expect(component.pageChange.emit).toHaveBeenCalledWith(2);
  });

  it('should go to next page', () => {
    spyOn(component.pageChange, 'emit');
    component.currentPage = 2;
    component.totalPages = 5;
    component.nextPage();
    expect(component.pageChange.emit).toHaveBeenCalledWith(3);
  });

  it('should not go below page 1', () => {
    spyOn(component.pageChange, 'emit');
    component.currentPage = 1;
    component.prevPage();
    expect(component.pageChange.emit).not.toHaveBeenCalled();
  });

  it('should not go above total pages', () => {
    spyOn(component.pageChange, 'emit');
    component.currentPage = 5;
    component.totalPages = 5;
    component.nextPage();
    expect(component.pageChange.emit).not.toHaveBeenCalled();
  });

  it('should not navigate when disabled', () => {
    spyOn(component.pageChange, 'emit');
    component.disabled = true;
    component.goToPage(2);
    expect(component.pageChange.emit).not.toHaveBeenCalled();
  });

  it('should calculate page numbers correctly', () => {
    component.currentPage = 5;
    component.totalPages = 10;
    const pages = component.pageNumbers;
    expect(pages).toContain(5);
    expect(pages.length).toBeLessThanOrEqual(5);
  });

  it('should not emit when clicking current page', () => {
    spyOn(component.pageChange, 'emit');
    component.currentPage = 3;
    component.totalPages = 5;
    component.goToPage(3);
    expect(component.pageChange.emit).not.toHaveBeenCalled();
  });
});
