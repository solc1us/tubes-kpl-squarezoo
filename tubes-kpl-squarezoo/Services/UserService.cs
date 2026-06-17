using System.Text.Json;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;

namespace tubes_kpl_squarezoo.Services;

public class UserService
{
    private Dictionary<string, User> _users;
    private string _filePath;

    public UserService(string filePath)
    {
        _filePath = filePath;
        _users = new Dictionary<string, User>();
        LoadFromFile();
    }

    public User CreateUser(string name, string phone, UserRole role = UserRole.Pelapor, string? password = null)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
            throw new ArgumentException("Name dan Phone tidak boleh kosong.");

        if (_users.ContainsKey(phone))
            return _users[phone];

        var newUser = new User(name, phone, role, password);
        _users.Add(phone, newUser);
        SaveToFile();

        return newUser;
    }

    public List<User> GetAll() => _users.Values.ToList();

    public User? GetById(Guid id)
    {
        return _users.Values.FirstOrDefault(u => u.UserId == id);
    }

    public User UpdateUser(Guid id, string name, string phone, UserRole role = UserRole.Pelapor, string? password = null)
    {
        var user = _users.Values.FirstOrDefault(u => u.UserId == id);
        if (user == null)
            throw new KeyNotFoundException("User tidak ditemukan.");

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
            throw new ArgumentException("Name dan Phone tidak boleh kosong.");

        if (_users.ContainsKey(phone) && _users[phone].UserId != id)
            throw new ArgumentException("Nomor HP sudah digunakan oleh user lain.");

        if (user.NoHP != phone)
            _users.Remove(user.NoHP);

        user.Name = name;
        user.NoHP = phone;
        user.Role = role;
        user.Password = password;
        _users[phone] = user;

        SaveToFile();
        return user;
    }

    public bool DeleteUser(Guid id)
    {
        var user = _users.Values.FirstOrDefault(u => u.UserId == id);
        if (user == null)
            return false;

        _users.Remove(user.NoHP);
        SaveToFile();
        return true;
    }

    public User? Login(string noHP, string password)
    {
        if (!_users.TryGetValue(noHP, out var user))
            return null;

        return user.Password == password ? user : null;
    }

    private void SaveToFile()
    {
        string? directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string jsonString = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonString);
    }

    private void LoadFromFile()
    {
        if (File.Exists(_filePath))
        {
            string jsonString = File.ReadAllText(_filePath);
            _users = JsonSerializer.Deserialize<Dictionary<string, User>>(jsonString) ?? new();
        }
    }
}
