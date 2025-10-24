import { TestBed } from '@angular/core/testing';
import { PlayerColumnsService } from './player-columns.service';

describe('PlayerColumnsService', () => {
  let service: PlayerColumnsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PlayerColumnsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return all headers', () => {
    const headers = service.getHeaders();
    expect(headers.length).toBeGreaterThan(0);
    expect(headers[0].key).toBe('PlayerName');
  });

  it('should return visible headers', () => {
    const headers = service.getVisibleHeaders();
    expect(headers.every(h => h.visible !== false)).toBe(true);
  });

  it('should toggle column visibility', () => {
    const headers = service.getHeaders();
    const nonStickyHeader = headers.find(h => !h.sticky);
    
    if (nonStickyHeader) {
      const initialVisibility = nonStickyHeader.visible;
      service.toggleColumn(nonStickyHeader);
      expect(nonStickyHeader.visible).toBe(!initialVisibility);
    }
  });

  it('should not toggle sticky column', () => {
    const headers = service.getHeaders();
    const stickyHeader = headers.find(h => h.sticky);
    
    if (stickyHeader) {
      const initialVisibility = stickyHeader.visible;
      service.toggleColumn(stickyHeader);
      expect(stickyHeader.visible).toBe(initialVisibility);
    }
  });

  it('should get column value', () => {
    const mockPlayer = {
      playerName: 'Test Player',
      cost: 100,
      totalPoints: 150
    };

    expect(service.getColumnValue(mockPlayer, 'PlayerName')).toBe('Test Player');
    expect(service.getColumnValue(mockPlayer, 'Cost')).toBe(100);
    expect(service.getColumnValue(mockPlayer, 'TotalPoints')).toBe(150);
  });

  it('should get cell class', () => {
    const headers = service.getHeaders();
    const playerNameHeader = headers.find(h => h.key === 'PlayerName');
    const costHeader = headers.find(h => h.key === 'Cost');
    const formHeader = headers.find(h => h.key === 'Form');
    
    if (playerNameHeader) {
      expect(service.getCellClass(playerNameHeader)).toContain('text-align-left');
      expect(service.getCellClass(playerNameHeader)).toContain('sticky-col');
    }
    
    if (costHeader) {
      expect(service.getCellClass(costHeader)).toContain('text-align-right');
      expect(service.getCellClass(costHeader)).toContain('numeric');
    }
    
    if (formHeader) {
      const mockPlayer = { form: 8 };
      expect(service.getCellClass(formHeader, mockPlayer)).toContain('form-high');
    }
  });

  it('should get form class', () => {
    expect(service.getFormClass(8)).toBe('form-high');
    expect(service.getFormClass(5)).toBe('form-mid');
    expect(service.getFormClass(2)).toBe('form-low');
  });

  it('should reset headers to default', () => {
    const headers = service.getHeaders();
    const nonStickyHeader = headers.find(h => !h.sticky);
    
    if (nonStickyHeader) {
      nonStickyHeader.visible = false;
      service.resetHeaders();
      const resetHeaders = service.getHeaders();
      expect(resetHeaders.every(h => h.visible !== false)).toBe(true);
    }
  });
});
