using System.Text.Json;
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

    public User CreateUser(string name, string phone)
    {
        // Pengecekan body request: pastikan name dan phone tidak kosong
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
            throw new ArgumentException("Name dan Phone tidak boleh kosong.");

        if (_users.ContainsKey(phone))
            return _users[phone]; // Kalo udah ada, balikin yang lama

        var newUser = new User(name, phone);
        _users.Add(phone, newUser);
        SaveToFile();

        return newUser;
    }

    public List<User> GetAll() => _users.Values.ToList();

    public User GetById(Guid id)
    {
        var user = _users.Values.FirstOrDefault(u => u.UserId == id);
        if (user == null)
            return null;
        return user;
    }

    // UpdateUser bisa dipakai untuk update nama atau nomor HP
    public User UpdateUser(Guid id, string name, string phone)
    {
        var user = _users.Values.FirstOrDefault(u => u.UserId == id);
        if (user == null)
            throw new KeyNotFoundException("User tidak ditemukan.");

        // Pengecekan body request: pastikan name dan phone tidak kosong
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
            throw new ArgumentException("Name dan Phone tidak boleh kosong.");

        // Kalo nomor HP baru udah dipakai sama user lain, gak boleh update
        if (_users.ContainsKey(phone) && _users[phone].UserId != id)
            throw new ArgumentException("Nomor HP sudah digunakan oleh user lain.");

        // Hapus user lama dari dictionary kalo nomor HP berubah
        if (user.NoHP != phone)
            _users.Remove(user.NoHP);

        user.Name = name;
        user.NoHP = phone;
        _users[phone] = user; // Update dengan nomor HP baru

        SaveToFile();
        return user;

    }

    // Delete user berdasarkan ID
    public bool DeleteUser(Guid id)
    {
        var user = _users.Values.FirstOrDefault(u => u.UserId == id);
        if (user == null)
            return false;
        _users.Remove(user.NoHP);
        SaveToFile();
        return true;
    }

    private void SaveToFile()
    {
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